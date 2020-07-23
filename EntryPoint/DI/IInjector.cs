namespace EntryPoint.DI
{
	public interface IInjector
	{
		T Get<T>();
	}
}