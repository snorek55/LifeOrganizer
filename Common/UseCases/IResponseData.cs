namespace Common.UseCases
{
	public interface IResponseData<T> : IResponse
	{
		T Data { get; set; }
	}
}