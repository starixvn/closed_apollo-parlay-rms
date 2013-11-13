// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Web
{
    using System;
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Core.Constants;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// Application session.
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>Current user.</value>
        public static User CurrentUser
        {
            get
            {
                return SessionManager.Get<User>(CommonConstants.CurrentLogInUser);
            }

            set
            {
                SessionManager.Set<User>(CommonConstants.CurrentLogInUser, value);
            }
        }

        /// <summary>
        /// Gets or sets menu of current login user.
        /// </summary>
        /// <value>Current user's menus.</value>
        public static IList<MenuItem> CurrentUserMenu
        {
            get
            {
                return SessionManager.Get<IList<MenuItem>>(CommonConstants.CurrentUserMenu);
            }

            set
            {
                SessionManager.Set<IList<MenuItem>>(CommonConstants.CurrentUserMenu, value);
            }
        }

        /// <summary>
        /// Gets or sets the role access.
        /// </summary>
        /// <value>The role access of current user.</value>
        public static Dictionary<string, int> RoleAccess
        {
            get
            {
                try
                {
                    return SessionManager.Get<Dictionary<string, int>>(CommonConstants.CurrentUserRoleAcess);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            set
            {
                SessionManager.Set<Dictionary<string, int>>(CommonConstants.CurrentUserRoleAcess, value);
            }
        }

        /// <summary>
        /// Gets or sets current working menu url.
        /// </summary>
        /// <value>Current working menu.</value>
        public static MenuItem CurrentWorkingOnMenu
        {
            get
            {
                return SessionManager.Get<MenuItem>(CommonConstants.CurrentWorkingOnMenu);
            }

            set
            {
                SessionManager.Set<MenuItem>(CommonConstants.CurrentWorkingOnMenu, value);
            }
        }

        /// <summary>
        /// Gets or sets IsTrackedMenu: determine current working on menu is
        /// tracked for page tracking or not.
        /// </summary>
        /// <value>Current action name.</value>
        public static string CurrentActionName
        {
            get
            {
                return SessionManager.Get<string>(CommonConstants.CurrentActionName);
            }

            set
            {
                SessionManager.Set<string>(CommonConstants.CurrentActionName, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current IP is in Vietnam.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current IP is in Vietnam; otherwise, <c>false</c>.
        /// </value>
        public static bool ViewIPVN
        {
            get
            {
                return SessionManager.Get<bool>(CommonConstants.ViewIPVN);
            }

            set
            {
                SessionManager.Set<bool>(CommonConstants.ViewIPVN, value);
            }
        }
    }
}