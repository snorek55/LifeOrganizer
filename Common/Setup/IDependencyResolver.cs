using Microsoft.Extensions.DependencyInjection;

namespace Common.Setup
{
	public interface IDependencyResolver
	{
		void Setup(IServiceCollection services);
	}
}