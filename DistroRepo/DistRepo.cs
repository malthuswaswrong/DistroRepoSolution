using Microsoft.Extensions.Options;

namespace DistroRepo;

public class DistRepo
{
    private readonly IOptions<DistRepoOptions> options;

    public DistRepo(IOptions<DistRepoOptions> options)
    {
        this.options = options;
    }
}
