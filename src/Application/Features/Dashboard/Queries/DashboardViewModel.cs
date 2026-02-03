using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class DashboardViewModel
    {
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public int Value { get; set; }
        public List<KPI> Details { get; set; }
        //public List<KPI> Programs { get; set; }
        //public List<KPI> Projects { get; set; }
        //public List<KPI> RegisteredEvents { get; set; }
        //public List<KPI> Articles { get; set; }
        //public List<KPI> Modules { get; set; }
        //public List<KPI> Sections { get; set; }
        //public List<KPI> Surveys { get; set; }
        //public List<KPI> ProjectIdeas { get; set; }
        //public List<DashboardProgram> DashboardPrograms { get; set; }
        //public List<DashboardModule> DashboardModules { get; set; }

        public class KPI
        {
            public string Name { get; set; }
            public string Name_AR { get; set; }
            public int Value { get; set; }
            public int Order { get; set; }

        }
        //public class Program
        //{
        //    public string Name { get; set; }
        //    public int Value { get; set; }
        //    public int Order { get; set; }

        //}
        //public class Event
        //{
        //    public string Name { get; set; }
        //    public int Value { get; set; }
        //    public int Order { get; set; }

        //}
        //public class Article
        //{
        //    public string Name { get; set; }
        //    public int Value { get; set; }
        //    public int Order { get; set; }

        //}
        //public class Survey
        //{
        //    public string Name { get; set; }
        //    public int Value { get; set; }
        //    public int Order { get; set; }

        //}
        //public class ProjectIdea
        //{
        //    public string Name { get; set; }
        //    public int Value { get; set; }
        //    public int Order { get; set; }

        //}
        //public class DashboardProgram
        //{
        //    public Guid ID { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public decimal Value { get; set; }
        //    public string URL { get; set; }

        //}
        //public class DashboardModule
        //{
        //    public Guid ID { get; set; }
        //    public Guid ProgramId { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public decimal Value { get; set; }

        //}
    }
}
