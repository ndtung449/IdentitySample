namespace IdentitySample.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using IdentitySample.Data.Services;
    using IdentitySample.Models;
    using Microsoft.AspNetCore.Authorization;
    using IdentitySample.Authorization;
    using System.Linq.Expressions;
    using System;
    using IdentitySample.Data.Entities;

    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<ActionResult> Index()
        {
            Expression<Func<Article, bool>> filter = null;
            if (User.IsInRole(Constants.AdminRole))
            {
                filter = null;
            }
            else if (User.IsInRole(Constants.UserModRole))
            {
                filter = article => article.IsActivated == true || article.CreateBy == User.Identity.Name;
            }
            else
            {
                filter = article => article.IsActivated == true;
            }

            var articles = await _articleService.GetAsync(filter);
            return View(articles);
        }

        public async Task<ActionResult> Details(string id)
        {
            var article = await _articleService.GetByIdAsync(id);
            return View(article);
        }

        //Role based authorization
        [Authorize(Roles = "Admin, UserMod")]
        public ActionResult Create()
        {
            return View();
        }

        //Policy based authorization
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdminOrUserModPolicy")]
        public async Task<ActionResult> Create(ArticleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreateBy = User.Identity.Name;
                    await _articleService.AddAsync(model);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(model);
        }

        //Resource based authorization
        public async Task<ActionResult> Edit(string id)
        {
            var isAuthorized = await _articleService.AuthorizeAsync(User, id, OperationRequirements.Update);
            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            var articleToUpdate = await _articleService.GetByIdAsync(id);
            return View(articleToUpdate);
        }

        //Resource based authorization
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAuthorized = await _articleService.AuthorizeAsync(User, model.Id, OperationRequirements.Update);
                if (!isAuthorized)
                {
                    return new ChallengeResult();
                }

                try
                {
                    await _articleService.UpdateAsync(model);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                }
            }

            return View(model);
        }

        //Policy based authorization
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(string id, bool deleteError = false)
        {
            var articleToDelete = await _articleService.GetByIdAsync(id);
            if (articleToDelete == null)
            {
                return NotFound();
            }

            if (deleteError)
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(articleToDelete);
        }

        //Policy based authorization
        [Authorize(Policy = "AdminOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _articleService.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(id, true);
            }
        }

        //Resource based authorization
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetStatus(string id, bool isActivated)
        {
            var requirement = isActivated ? OperationRequirements.Activate : OperationRequirements.Deactivate;
            var isAuthorized = await _articleService.AuthorizeAsync(User, id, requirement);
            if (!isAuthorized)
            {
                return new ChallengeResult();
            }

            var articleViewModel = await _articleService.GetByIdAsync(id);
            articleViewModel.IsActivated = isActivated;
            await _articleService.UpdateAsync(articleViewModel);
            return RedirectToAction("Index");
        }
    }
}