namespace Common.UseCases
{
	public class DataResponseBase<TData> : ResponseBase, IResponseData<TData>
	{
		//TODO: must implement this in all responses
		public TData Data { get; set; }

		public DataResponseBase()
		{
		}

		public DataResponseBase(TData data)
		{
			this.Data = data;
		}

		public DataResponseBase(string error)
		{
			Error = error;
		}
	}
}