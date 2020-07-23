namespace Organizers.Common.UseCases
{
	public interface IResponse
	{
		public bool HasError { get; }
		public string Error { get; }
	}
}