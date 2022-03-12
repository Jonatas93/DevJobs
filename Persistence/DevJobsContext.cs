using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevJobs.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.API.Persistence
{
    public class DevJobsContext : DbContext
    {
        public DevJobsContext(DbContextOptions<DevJobsContext> options) : base(options)
        {
            
        }
        public DbSet<JobVacancy> JobVacancies {get; set;}
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            //Builder: Define configurações para as propriedades/colunas da tabela do banco de dados
            
            builder.Entity<JobVacancy>(e => 
            {
                //Define a PK
                e.HasKey(jv => jv.Id);
                
                //Definição de chave estrangeira (1 JobVacancy tem várias applications, 1 application pode ser aplicada a 1 vaga, a chave estrangeira é IdJobVacancy, o onDelete(restrict) não permite excluir JobVacancy se tiver applications vinculadas)
                e.HasMany(jv => jv.Applications)
                    .WithOne()
                    .HasForeignKey(ja => ja.IdJobVacancy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<JobApplication>(e => 
            {
                //Define a PK
                e.HasKey(ja => ja.Id);
            });
        }
    }
}