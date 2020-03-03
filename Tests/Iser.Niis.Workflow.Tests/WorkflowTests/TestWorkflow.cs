using NUnit.Framework;

namespace Iserv.Niis.Workflow.Tests.WorkflowTests
{
    //todo: Просто класс. для натсройки. удалить после того как начнем писать тесты
    [TestFixture]
    public class TestWorkflow: BaseWorkflowTest
    {
        [Test]
        public void FillDataBase_Test_Success()
        {
            //Так будем заполнять тестовую БД перед погоном теста каждого процесса
            FillTestDataBase();
            //Так будем очищать тестовую БД после каждого прогона каждого процесса
            ClearTestDataBase();
        }
    }
}
