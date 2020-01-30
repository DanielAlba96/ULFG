using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace ULFG.Forms.Behaviors.Events.ViewsEventsBehaviors
{
    /// <summary>
    /// <see cref="BehaviorEventBase{Switch}"/> para todos los eventos del componente <see cref="Switch"/>
    /// </summary>
    public class SwitchEventBehavior : BehaviorEventBase<Switch>
    {
        Delegate eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(SwitchEventBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(SwitchEventBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(SwitchEventBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(SwitchEventBehavior), null);

        public string SwitchEventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand SwitchCommand
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object SwitchCommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter SwitchConverter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(Switch bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(SwitchEventName);
        }

        protected override void OnDetachingFrom(Switch bindable)
        {
            DeregisterEvent(SwitchEventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", SwitchEventName));
            }
            MethodInfo methodInfo = typeof(SwitchEventBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (eventHandler == null)
            {
                return;
            }
            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", SwitchEventName));
            }
            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        void OnEvent(object sender, object eventArgs)
        {
            if (SwitchCommand == null)
            {
                return;
            }

            object resolvedParameter;
            if (SwitchCommandParameter != null)
            {
                resolvedParameter = SwitchCommandParameter;
            }
            else if (SwitchConverter != null)
            {
                resolvedParameter = SwitchConverter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (SwitchCommand.CanExecute(resolvedParameter))
            {
                SwitchCommand.Execute(resolvedParameter);
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (SwitchEventBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}
