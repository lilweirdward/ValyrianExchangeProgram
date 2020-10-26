﻿using AutoMapper;
using Braavos.Core.Parsers.DataObjects;
using Braavos.Core.Repositories.DataObjects;
using System;

namespace Braavos.Function
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CnNation, Nation>()
                .ForMember(dest => dest.GovernmentType, opt => opt.MapFrom((src, dest) =>
                {
                    if (Enum.TryParse<GovernmentType>(src.GovernmentType.Replace(" ", ""), out var govType))
                        return govType;

                    return GovernmentType.Anarchy;
                }))
                .ForMember(dest => dest.Religion, opt => opt.MapFrom((src, dest) =>
                {
                    if (Enum.TryParse<Religion>(src.Religion.Replace(" ", ""), out var religion))
                        return religion;

                    return Religion.Mixed;
                }))
                .ForMember(dest => dest.Team, opt => opt.MapFrom((src, dest) =>
                {
                    if (Enum.TryParse<Team>(src.Team, out var team))
                        return team;

                    return Team.None;
                }))
                .ForMember(dest => dest.CruiseMissiles, opt => opt.MapFrom(src => src.Cruise))
                .ForMember(dest => dest.RecentActivity, opt => opt.MapFrom((src, dest) =>
                {
                    if (Enum.TryParse<RecentActivity>(src.Activity.Replace(" ", ""), out var recentActivity))
                        return recentActivity;

                    return RecentActivity.ActiveMoreThanThreeWeeksAgo;
                }));
        }
    }
}
