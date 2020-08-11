namespace Common.Setup
{
	public interface IInjector
	{
		T Get<T>();
	}
}