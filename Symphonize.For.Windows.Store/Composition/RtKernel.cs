using Caliburn.Micro;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Symphonize.For.Windows.Store.Composition
{
    public class RtKernel : StandardKernel
    {
        /// <summary>
        /// Registers the Caliburn.Micro navigation service with the container.
        /// </summary>
        /// <param name="rootFrame">The application root frame.</param>
        /// <param name="treatViewAsLoaded">if set to <c>true</c> [treat view as loaded].</param>
        public INavigationService RegisterNavigationService(Frame rootFrame, bool treatViewAsLoaded = false)
        {
            if (GetBindings(typeof(INavigationService)).Any())
                return this.Get<INavigationService>();

            if (rootFrame == null)
                throw new ArgumentNullException("rootFrame");

            var frameAdapter = new FrameAdapter(rootFrame, treatViewAsLoaded);

            Bind<INavigationService>().ToConstant(frameAdapter).InSingletonScope();

            return frameAdapter;
        }

        public ISharingService RegisterSharingService()
        {
            if (GetBindings(typeof(ISharingService)).Any())
                return this.Get<ISharingService>();

            var sharingService = new SharingService();

            Bind<ISharingService>().ToConstant(sharingService).InSingletonScope();

            return sharingService;
        }

        public ISettingsService RegisterSettingsService()
        {
            if (GetBindings(typeof(ISettingsService)).Any())
                return this.Get<ISettingsService>();

            if (!GetBindings(typeof(ISettingsWindowManager)).Any())
                Bind<ISettingsWindowManager>().ToConstant(new SettingsWindowManager()).InSingletonScope();

            var settingsService = new SettingsService(this.Get<ISettingsWindowManager>());
            
            Bind<ISettingsService>().ToConstant(settingsService).InSingletonScope();

            return settingsService;
        }
    }
}
