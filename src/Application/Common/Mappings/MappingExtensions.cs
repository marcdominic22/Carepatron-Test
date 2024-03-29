﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}
