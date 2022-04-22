using BlogCore.Core.Models;
using witeem.CoreHelper.DependencyInjection;
using Xunit;

namespace WiteemTest
{
    public class UnitTest1
    {
        [Fact]
        public void CreateDataBase()
        {
            DataBaseOperate.CreateTable();
        }
    }
}