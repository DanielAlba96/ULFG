using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace ULFG.Forms.Behaviors.Events.ViewsEventsBehaviors
{
    /// <summary>
    /// <see cref="BehaviorEventBase{ListView}"/> para todos los eventos del componente <see cref="ListView"/>
    /// </summary>
    public class ListViewEventBehavior: BehaviorEventBase<ListView>
    {
        Delegate eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(ListViewEventBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewEventBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(ListViewEventBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(ListViewEventBehavior), null);

        public string ListEventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand ListCommand
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object ListCommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter ListConverter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(ListEventName);
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            DeregisterEvent(ListEventName);
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
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", ListEventName));
            }
            MethodInfo methodInfo = typeof(ListViewEventBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
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
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", ListEventName));
            }
            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        void OnEvent(object sender, object eventArgs)
        {
            if (ListCommand == null)
            {
                return;
            }

            object resolvedParameter;
            if (ListCommandParameter != null)
            {
                resolvedParameter = ListCommandParameter;
            }
            else if (ListConverter != null)
            {
                resolvedParameter = ListConverter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (ListCommand.CanExecute(resolvedParameter))
            {
                ListCommand.Execute(resolvedParameter);
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (ListViewEventBehavior)bindable;
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
