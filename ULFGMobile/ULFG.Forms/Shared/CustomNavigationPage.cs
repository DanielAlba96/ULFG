using System;
using System.Collections.Generic;
using System.Text;
using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.Guilds.Views;
using ULFG.Forms.Network.Views;
using Xamarin.Forms;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// <see cref="NavigationPage"/> propia creada para sobrescribir el evento <see cref="NavigationPage.Popped"/>
    /// </summary>
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page content) : base(content)
        {
            Init();
        }
        /// <summary>
        /// Sobreescribe el evento <see cref="NavigationPage.Popped"/> para determinar si debe actualizar una vista o no
        /// </summary>
        private void Init()
        {
            Popped += (sender, e) =>
            {
                //Flag para actualizar al usar la master detail
                var guild = e.Page as GuildTabView;
                var network = e.Page as MyNetwork;

                if (guild != null)
                    guild.Updated = false;
                if (network != null)
                    network.Updated = false;
            };
        }
    }
}
