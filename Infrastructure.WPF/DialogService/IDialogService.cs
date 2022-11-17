namespace AssemblyBrowserGUI.ViewModel
{
    /// <summary>
    /// Provides basic communication between user and program:
    /// Open dialog and Messagebox
    /// </summary>
    public interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        bool OpenFileDialog();
    }
}
