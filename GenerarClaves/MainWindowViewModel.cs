using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace GenerarClaves
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string inputText = string.Empty;

        [ObservableProperty]
        private string outputText = string.Empty;

        [ObservableProperty]
        private string selectedScope = "Usuario Actual";

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string entropyText = "0102030405";

        public ObservableCollection<string> Scopes { get; } =
        [
            "Usuario Actual",
            "Máquina"
        ];

        private byte[]? GetEntropyBytes()
        {
            if (string.IsNullOrWhiteSpace(EntropyText))
            {
                ErrorMessage = "Por favor, ingrese una entropía válida.";
                return null;
            }

            // Convertir el texto a bytes usando UTF-8
            return Encoding.UTF8.GetBytes(EntropyText);
        }

        [RelayCommand]
        private void Encrypt()
        {
            try
            {
                ErrorMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(InputText))
                {
                    ErrorMessage = "Por favor, ingrese texto para cifrar.";
                    return;
                }

                byte[]? entropy = GetEntropyBytes();
                if (entropy == null) return;

                bool usarMaquina = SelectedScope == "Máquina";
                OutputText = DpapiHelper.Cifrar(InputText, entropy, usarMaquina);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al cifrar: {ex.Message}";
                OutputText = string.Empty;
            }
        }

        [RelayCommand]
        private void Decrypt()
        {
            try
            {
                ErrorMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(InputText))
                {
                    ErrorMessage = "Por favor, ingrese texto cifrado para descifrar.";
                    return;
                }

                byte[]? entropy = GetEntropyBytes();
                if (entropy == null) return;

                bool usarMaquina = SelectedScope == "Máquina";
                OutputText = DpapiHelper.Descifrar(InputText, entropy, usarMaquina);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al descifrar: {ex.Message}";
                OutputText = string.Empty;
            }
        }
    }
}