﻿using ULFG.Core.Data.Item;
using Xamarin.Forms;
using ULFG.Forms.PrivateChat.ViewModels;
using ULFG.Forms.Shared;

namespace ULFG.Forms.PrivateChat.Views
{
    /// <summary>
    /// <see cref="ChatView"/> que representa la vista del chat individual
    /// </summary>
    public class IndividualChatView : ChatView
    {
        public IndividualChatView(Chat chat): base()
        {
            BindingContext = new IndividualChatViewModel(Navigation, chat);

            this.SetBinding(ContentPage.TitleProperty, "Title");

            ToolbarItem leave = new ToolbarItem()
            {
                Text = "Abandonar",
                Order = ToolbarItemOrder.Secondary
            };
            leave.SetBinding(ToolbarItem.CommandProperty, "Leave");

            ToolbarItem profile = new ToolbarItem()
            {
                Text = "Ver Perfil",
                Order = ToolbarItemOrder.Secondary
            };
            profile.SetBinding(ToolbarItem.CommandProperty, "Profile");

            ToolbarItems.Add(profile);
            ToolbarItems.Add(leave);
        }
    }
}