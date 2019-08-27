using System.Collections.Generic;
using ManjuuInfrastructure.Log;
using ManjuuInfrastructure.Repository.Entity;
using Microsoft.EntityFrameworkCore;

// 避免 DbContext 线程问题
// Entity Framework Core 不支持在同一DbContext实例上运行多个并行操作。 这包括异步查询的并行执行以及从多个线程进行的任何显式并发使用。 因此, 应await立即异步调用, 或对并行DbContext执行的操作使用单独的实例。
// 当 EF Core 检测到并行使用某个DbContext实例时, 您将InvalidOperationException看到一条消息, 如下所示:
// 在上一个操作完成之前, 在此上下文上启动的第二个操作。 这通常是由使用同一个 DbContext 实例的不同线程引起的, 但不保证实例成员是线程安全的。

namespace ManjuuInfrastructure.Repository.Context {
    public class HealthManjuuCoreContext : DbContext
    {
        public DbSet<JobConfiguration> JobConfigurations { get; set; }

        public DbSet<MachineInfo> MachineInfos { get; set; }

        public HealthManjuuCoreContext()
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            //optionsBuilder.UseLoggerFactory(new EFCoreConsoleLogFactory().FactoryInstance);
            optionsBuilder.UseSqlite(@"Filename=../HealthManjuuCore.db");


            




        }

    }
}