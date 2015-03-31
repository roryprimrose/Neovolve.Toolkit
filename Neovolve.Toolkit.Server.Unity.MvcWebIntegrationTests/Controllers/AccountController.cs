namespace Neovolve.Toolkit.Server.Unity.MvcWebIntegrationTests.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Neovolve.Toolkit.Server.Unity.MvcWebIntegrationTests.Models;

    /// <summary>
    /// The account controller.
    /// </summary>
    [HandleError]
    public class AccountController : Controller
    {
        // **************************************
        // URL: /Account/LogOn
        // **************************************

        /// <summary>
        /// The change password.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        /// <summary>
        /// The change password.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        /// <summary>
        /// The change password success.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// The log off.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The log on.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [ValidateAntiForgeryToken]
        public ActionResult LogOn()
        {
            return View();
        }

        /// <summary>
        /// The log on.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnModel model, String returnUrl)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(String.Empty, "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        // **************************************
        // URL: /Account/Register
        // **************************************

        /// <summary>
        /// The register.
        /// </summary>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [ValidateAntiForgeryToken]
        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// A <see cref="ActionResult"/> instance.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(String.Empty, AccountValidation.ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }

            base.Initialize(requestContext);
        }

        /// <summary>
        ///   Gets or sets FormsService.
        /// </summary>
        /// <value>
        ///   The forms service.
        /// </value>
        public IFormsAuthenticationService FormsService
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets MembershipService.
        /// </summary>
        /// <value>
        ///   The membership service.
        /// </value>
        public IMembershipService MembershipService
        {
            get;
            set;
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************
    }
}