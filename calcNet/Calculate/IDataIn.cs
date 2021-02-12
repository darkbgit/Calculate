namespace calcNet
{
    interface IDataIn
    {
        public bool IsDataGood { get;  }

        public string Error { get; }
    }

}
