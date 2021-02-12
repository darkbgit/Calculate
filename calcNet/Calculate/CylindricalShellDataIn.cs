namespace calcNet
{
    class CylindricalShellDataIn : ShellDataIn, IDataIn
    {

        public void CheckData()
        {
            if (ErrorList?.Count > 0)
            {
                IsDataGood = false;
            }
            else
            {
                IsDataGood = true;
            }
        }

        public bool IsDataGood { get; set; }

        public string Error
        {
            get => error; 
            set
            {
                error += value;
            }
        }

        private string error;

    }

}
