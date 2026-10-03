using System.Globalization;
using static GlucoMan.Common;

#if !MATH_TESTS_ONLY
namespace gamon
{
    internal class UiAccuracy
    {
        private sealed class AccuracyOption
        {
            public QualitativeAccuracy Value { get; init; }
            public string Display { get; init; } = string.Empty;
            public override string ToString() => Display;
        }

        private Entry txtQuantitative;
        private Picker cmbQualitative;
        private int halfInterval = ((int)QualitativeAccuracy.Perfect - (int)QualitativeAccuracy.Null) / 20;
        private bool editingNumericAccuracy = false;
        private bool userChoseQualitative = false;

        internal QualitativeAccuracy GetQualitativeAccuracyGivenQuantitavive(double? NumericalAccuracy)
        {
            if (NumericalAccuracy < 0 || NumericalAccuracy > 100)
            {
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy <= halfInterval)
            {
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + halfInterval)
            {
                return QualitativeAccuracy.AlmostNull;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + halfInterval)
            {
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + halfInterval)
            {
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + halfInterval)
            {
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + halfInterval)
            {
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + halfInterval)
            {
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + halfInterval)
            {
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + halfInterval)
            {
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + halfInterval)
            {
                return QualitativeAccuracy.Outstanding;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + halfInterval)
            {
                return QualitativeAccuracy.Perfect;
            }
            else
            {
                return QualitativeAccuracy.NotSet;
            }
        }
        internal UiAccuracy(Entry TextBox, Picker Combo)
        {
            txtQuantitative = TextBox;
            cmbQualitative = Combo;

            ConfigureLocalizedAccuracyItems();

            // hookup useful events 
            Combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
            TextBox.TextChanged += TextBox_TextChanged;
        }

        private void ConfigureLocalizedAccuracyItems()
        {
            bool isItalian = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("it", StringComparison.OrdinalIgnoreCase);

            var options = Enum.GetValues(typeof(QualitativeAccuracy))
                .Cast<QualitativeAccuracy>()
                .Select(v => new AccuracyOption
                {
                    Value = v,
                    Display = isItalian ? GetItalianLabel(v) : v.ToString()
                })
                .ToList();

            cmbQualitative.ItemsSource = options;
            cmbQualitative.ItemDisplayBinding = new Binding(nameof(AccuracyOption.Display));
        }

        private static string GetItalianLabel(QualitativeAccuracy value)
        {
            return value switch
            {
                QualitativeAccuracy.NotSet => "Non impostata",
                QualitativeAccuracy.Null => "Nulla",
                QualitativeAccuracy.AlmostNull => "Quasi nulla",
                QualitativeAccuracy.VeryBad => "Molto scarsa",
                QualitativeAccuracy.Bad => "Scarsa",
                QualitativeAccuracy.Poor => "Mediocre",
                QualitativeAccuracy.AlmostSufficient => "Quasi sufficiente",
                QualitativeAccuracy.Sufficient => "Sufficiente",
                QualitativeAccuracy.Satisfactory => "Soddisfacente",
                QualitativeAccuracy.Good => "Buona",
                QualitativeAccuracy.Outstanding => "Eccellente",
                QualitativeAccuracy.Perfect => "Perfetta",
                _ => value.ToString()
            };
        }

        internal object? GetPickerItemForAccuracy(QualitativeAccuracy value)
        {
            if (cmbQualitative.ItemsSource is IEnumerable<AccuracyOption> options)
                return options.FirstOrDefault(o => o.Value == value);

            return value;
        }

        internal QualitativeAccuracy? GetSelectedAccuracyValue()
        {
            if (cmbQualitative.SelectedItem is AccuracyOption selected)
                return selected.Value;

            if (cmbQualitative.SelectedItem is QualitativeAccuracy qa)
                return qa;

            return null;
        }
        private void TextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            double acc;
            //if (!txtQuantitative.IsLoaded || userChoseQualitative)
            if (userChoseQualitative)
                    return;
            editingNumericAccuracy = true;
            if (!double.TryParse(txtQuantitative.Text, out acc))
            {
                cmbQualitative.SelectedItem = null;
                txtQuantitative.Text = "";
                txtQuantitative.BackgroundColor = Colors.White;
                txtQuantitative.TextColor = Colors.Black;
                cmbQualitative.BackgroundColor =  Colors.White;
                cmbQualitative.TextColor = Colors.Black;
            }
            else
            {
                if (Double.IsFinite(acc) && acc >= 0 && acc <= 100)
                {
                    var option = GetPickerItemForAccuracy(GetQualitativeAccuracyGivenQuantitavive(acc));
                    cmbQualitative.SelectedItem = option;
                    txtQuantitative.BackgroundColor = AccuracyBackColor(acc);
                    txtQuantitative.TextColor = AccuracyForeColor(acc);
                    cmbQualitative.BackgroundColor = AccuracyBackColor(acc);
                    cmbQualitative.TextColor = AccuracyForeColor(acc);
                }
                else
                {
                    cmbQualitative.SelectedItem = null;
                    txtQuantitative.Text = "";
                    txtQuantitative.BackgroundColor = Colors.White;
                    txtQuantitative.TextColor = Colors.Black;
                    cmbQualitative.BackgroundColor = Colors.White;
                    cmbQualitative.TextColor = Colors.Black;
                }
            }
            editingNumericAccuracy = false;
        }
        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbQualitative.IsLoaded || editingNumericAccuracy)
                return;
            userChoseQualitative = true;
            var selectedValue = GetSelectedAccuracyValue();
            if (selectedValue.HasValue)
            {
                int acc = (int)selectedValue.Value;
                txtQuantitative.Text = (acc).ToString();
                txtQuantitative.BackgroundColor = AccuracyBackColor(acc);
                txtQuantitative.TextColor = AccuracyForeColor(acc);
                cmbQualitative.BackgroundColor = AccuracyBackColor(acc);
                cmbQualitative.TextColor = AccuracyForeColor(acc);
                // the value (int) associated with the QualitativeAccuracy is given to the numerical accuracy
                int accuracyNumber = (int)selectedValue.Value;
            }
            userChoseQualitative = false;
        }
        internal Color AccuracyBackColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Colors.Red;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                c = Colors.DarkRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                c = Colors.MediumVioletRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                c = Colors.OrangeRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                c = Colors.Orange;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                c = Colors.Yellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                c = Colors.YellowGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                c = Colors.GreenYellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Colors.Lime;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding + 5)
            {
                c = Colors.DarkSeaGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
            {
                c = Colors.Green;
            }
            else
            {
                c = Colors.MintCream;
            }
            return c;
        }
        internal Color AccuracyForeColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull + 5)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad + 5)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad + 5)
            {
                c = Colors.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good + 5)
            {
                c = Colors.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect + 5)
            {
                c = Colors.White;
            }
            else
            {
                c = Colors.White;
            }
            return c;
        }
    }
}
#endif

