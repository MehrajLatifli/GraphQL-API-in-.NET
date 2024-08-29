using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Services.Instructors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly InstructorRepository _instructorRepository;

        public InstructorDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions? options = null, InstructorRepository instructorRepository = null)
            : base(batchScheduler, options)
        {
            _instructorRepository = instructorRepository ?? throw new ArgumentNullException(nameof(instructorRepository));
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            IEnumerable<InstructorDTO> instructors = await _instructorRepository.GetManyByIds(keys);
            return instructors.ToDictionary(dto => dto.Id);
        }
    }
}
