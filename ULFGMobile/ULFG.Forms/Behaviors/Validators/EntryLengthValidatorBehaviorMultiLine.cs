using Xamarin.Forms;

namespace ULFG.Forms.Behaviors.Validators
{
    /// <summary>
    /// <see cref="Behavior{Editor}"/> que valida el tamaño maximo de un campo de texto multilinea
    /// </summary>
    public class EntryLengthValidatorBehaviorMultiLine : Behavior<Editor>
    {
        /// <summary>
        /// Máxima longuitud permitida de la cadena
        /// </summary>
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Editor)sender;

            // if Entry text is longer then valid length
            if (entry.Text.Length > this.MaxLength)
            {
                string entryText = entry.Text;

                entryText = entryText.Remove(entryText.Length - 1); // remove last char

                entry.Text = entryText;
            }
        }
    }
}
