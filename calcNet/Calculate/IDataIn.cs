namespace calcNet
{
    public interface IDataIn
    {
        void CheckData();

        public bool IsDataGood { get;  }

        public string Error { get; }
    }

}
