// --------------------------------------------------------------------------
// <copyright file="MenuService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.ParlayRMS.Core.Web;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// Menu Service.
    /// </summary>
    public class MenuService : BaseService
    {
        public const char UrlSeparator = '/';

        /// <summary>
        /// Gets menu path from request file path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="applicationPath">Application path.</param>
        /// <returns>Menu name.</returns>
        public static string GetMenuPathFromRequestFilePath(string filePath, string applicationPath)
        {
            filePath = filePath.Trim();
            applicationPath = applicationPath.Trim();
            var menuPath = filePath;

            if (!string.IsNullOrEmpty(applicationPath)
                && menuPath.StartsWith(applicationPath)
                && (AppSession.CurrentUserMenu != null))
            {
                menuPath = menuPath.Remove(0, applicationPath.Length);
            }

            if (!menuPath.StartsWith(UrlSeparator.ToString()))
            {
                menuPath = UrlSeparator + menuPath;
            }

            return menuPath;
        }

        /// <summary>
        /// Gets menu by ID.
        /// </summary>
        /// <param name="menuId">Menu ID value.</param>
        /// <returns>Menu item.</returns>
        public MenuItem GetMenuById(int menuId)
        {
            if (AppSession.CurrentUserMenu != null)
            {
                MenuItem menu = AppSession.CurrentUserMenu.FirstOrDefault<MenuItem>(m => m.Id == menuId);
                return menu;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets menu by menu URL.
        /// </summary>
        /// <param name="menuURL">Menu URL string.</param>
        /// <returns>Menu item.</returns>
        public MenuItem GetMenuByURL(string menuURL)
        {
            if (!string.IsNullOrEmpty(menuURL)
                && !string.Equals(menuURL, UrlSeparator.ToString())
                && (AppSession.CurrentUserMenu != null))
            {
                MenuItem menu = AppSession.CurrentUserMenu.FirstOrDefault<MenuItem>(m => m.Url.StartsWith(menuURL));
                return menu;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets menu item by menu URL from cached list.
        /// </summary>
        /// <param name="menuURL">Menu URL string.</param>
        /// <param name="menuList">List of cached menu items.</param>
        /// <returns>Menu item.</returns>
        public MenuItem GetMenuByURL(string menuURL, IList<MenuItem> menuList)
        {
            if (!string.IsNullOrEmpty(menuURL)
                && !string.Equals(menuURL, UrlSeparator.ToString())
                && (AppSession.CurrentUserMenu != null))
            {
                MenuItem menu = menuList.FirstOrDefault<MenuItem>(m => m.Url.StartsWith(menuURL));
                return menu;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets menu name by menu URL.
        /// </summary>
        /// <param name="menuURL">Menu URL string.</param>
        /// <returns>Name of menu.</returns>
        public string GetMenuNameByURL(string menuURL)
        {
            if (!string.IsNullOrEmpty(menuURL)
                    && !string.Equals(menuURL, UrlSeparator.ToString())
                    && (AppSession.CurrentUserMenu != null))
            {
                MenuItem menu = AppSession.CurrentUserMenu.FirstOrDefault<MenuItem>(m => m.Url.StartsWith(menuURL));
                return menu.Name;
            }
            else
            {
                return null;
            }
        }
    }
}