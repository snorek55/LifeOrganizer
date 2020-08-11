using Microsoft.Extensions.DependencyInjection;

namespace Common.DI
{
	public interface IDependencyResolver
	{
		void Setup(IServiceCollection services);
	}
}