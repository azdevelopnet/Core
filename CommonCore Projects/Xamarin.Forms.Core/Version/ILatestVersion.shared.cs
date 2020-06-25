﻿using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
    /// <summary>
    /// LatestVersion plugin
    /// </summary>
    public interface ILatestVersion
    {
        /// <summary>
        /// Gets the version number of the current app's installed version.
        /// </summary>
        /// <value>The current app's installed version number.</value>
        string InstalledVersionNumber { get; }

        /// <summary>
        /// Checks if the current app is the latest version available in the public store.
        /// </summary>
        /// <returns>True if the current app is the latest version available, false otherwise.</returns>
        Task<bool> IsUsingLatestVersion();

        /// <summary>
        /// Gets the version number of the current app's latest version available in the public store.
        /// </summary>
        /// <returns>The current app's latest version number.</returns>
        Task<string> GetLatestVersionNumber();

        /// <summary>
        /// Opens the current app in the public store.
        /// </summary>
        Task OpenAppInStore();
    }
}
