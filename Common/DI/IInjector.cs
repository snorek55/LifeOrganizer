namespace Common.DI
{
	public interface IInjector
	{
		T Get<T>();
	}
}