namespace calcNet
{
    class CylindricalShellDataIn : ShellDataIn, IDataIn
    {
        public CylindricalShellDataIn()
            : base(ShellType.Cylindrical)
        {

        }

        public void CheckData()
        {
            IsDataGood = !(ErrorList?.Count > 0);
        }

        public bool IsDataGood { get; set; }

        public string Error
        {
            get => error; 
            set => error += value;
        }

        private string error;

    }

}
