using Force.DeepCloner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetComparer.Common;
using SetComparer.Sets;
using System;
using System.Collections.Generic;
using System.IO;

namespace SetComparer.Comparers.Tests
{
    [TestClass()]
    public class IntSetComparerTests
    {
        IntSetComparer comp;
        PrivateObject privateObjectComp;
        IntSetComparer compEmpty;
        PrivateObject privateObjectCompEmpty;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            comp = new IntSetComparer();
            privateObjectComp = new PrivateObject(comp);
            compEmpty = new IntSetComparer();
            privateObjectCompEmpty = new PrivateObject(compEmpty);

            List<Sets.ISet<int>> uniqueSets = new List<Sets.ISet<int>>
            {
                new IntSet("1,2,3") { DuplicatesCount = 2 },
                new IntSet("56,69,26,0"),
                new IntSet("2,156") { DuplicatesCount = 1 },
                new IntSet("32")
            };
            privateObjectComp.SetField("sets", uniqueSets);

            List<ReportItem> report = new List<ReportItem>
            {
                new ReportItem("56,,0", DateTime.Parse("2008-09-15T09:30:41.7752486Z")),
                new ReportItem("null", DateTime.Parse("2008-09-15T09:30:42.7752486Z")),
                new ReportItem("156,ab", DateTime.Parse("2008-09-15T09:30:43.7752486Z"))
            };
            privateObjectComp.SetField("report", report);
        }

        [TestMethod()]
        [DeploymentItem(@"TestFiles\input.txt")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", 
            @"|DataDirectory|\input.txt",
            "input#txt",
            DataAccessMethod.Sequential
        )]
        [TestCategory("WithFiles")]
        [TestCategory("WithEmptyComp")]
        public void AddUniqueSetStringDuplicatesTest()
        {
            // arrange
            string setA = TestContext.DataRow["SetA"].ToString();
            string setB = TestContext.DataRow["SetB"].ToString();
            bool expected = bool.Parse(TestContext.DataRow["SetBIsUnique"].ToString());
            // act
            compEmpty.AddUniqueSet(setA);
            bool actual = compEmpty.AddUniqueSet(setB);
            // assert
            Assert.AreEqual(expected, actual, 
                "SetA = {0} ; SetB = {1} ; Expected = {2} ; Actual = {3}", 
                setA, setB, expected, actual);           
        }

        [TestMethod()]
        [TestCategory("WithEmptyComp")]
        public void AddUniqueSetStringTest()
        {
            // arrange
            const string INPUT = "123,369,2,26";
            const bool EXPECTED = true;
            // act
            bool actual = compEmpty.AddUniqueSet(INPUT);
            // assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [TestMethod()]
        [TestCategory("WithEmptyComp")]
        public void AddUniqueSetWrongStringTest()
        {
            // arrange
            const string INPUT = "123,369,2,26,";
            const bool EXPECTED = false;
            // act
            bool actual = compEmpty.AddUniqueSet(INPUT);
            // assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [TestMethod()]
        public void AddUniqueSetUniquesListTest()
        {
            // arrange
            List<Sets.ISet<int>> uniqueSets = new List<Sets.ISet<int>>
            {
                new IntSet("1,2,3"),
                new IntSet("56,69,26,0"),
                new IntSet("2,156"),
                new IntSet("32")
            };
            List<Sets.ISet<int>> uniqueSetsCopy = uniqueSets.DeepClone();
            int expectedAddedCount = uniqueSets.Count;
            int actualAddedCount = 0; 

            // act
            foreach(Sets.ISet<int> set in uniqueSetsCopy)
            {
                if (compEmpty.AddUniqueSet(set))
                    actualAddedCount++;
            }

            // assert
            Assert.AreEqual(expectedAddedCount, actualAddedCount);

            List<Sets.ISet<int>> compSets = (List<Sets.ISet<int>>)privateObjectCompEmpty.GetField("sets");
            Assert.AreEqual(uniqueSets.Count, compSets.Count);

            for (int i = 0; i < uniqueSets.Count; i++)
            {
                CollectionAssert.AreEquivalent(uniqueSets[i].Set, compSets[i].Set);
            }
        }

        [TestMethod()]
        public void GetDuplicatesCountTest()
        {            
            // arrange
            const int EXPECTED = 3;
            // act
            int actual = comp.GetDuplicatesCount();
            // assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [TestMethod()]
        [TestCategory("WithEmptyComp")]
        public void GetDuplicatesCountEmptyTest()
        {
            // arrange
            const int EXPECTED = 0;
            // act
            int actual = compEmpty.GetDuplicatesCount();
            // assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [TestMethod()]
        public void GetUniquesCountTest()
        {
            // arrange
            const int EXPECTED = 4;
            // act
            int actual = comp.GetUniquesCount();
            // assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [TestMethod()]
        [TestCategory("WithEmptyComp")]
        public void GetUniquesCountEmptyTest()
        {
            // arrange
            const int EXPECTED = 0;
            // act
            int actual = compEmpty.GetUniquesCount();
            // assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [TestMethod()]
        public void GetMostFrequentSetTest()
        {
            // arrange
            List<int> expected = new List<int> { 1, 2, 3 };
            // act
            List<int> actual = comp.GetMostFrequentSet().Set;
            // assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod()]
        [TestCategory("WithEmptyComp")]
        public void GetMostFrequentSetEmptyTest()
        {
            // arrange
            // --
            // act
            Sets.ISet<int> actual = compEmpty.GetMostFrequentSet();
            // assert
            Assert.IsNull(actual);
        }

        [TestMethod()]
        [DeploymentItem(@"TestFiles\IntInvalidInputsReport.xml")]
        [TestCategory("WithFiles")]
        public void GetInvalidInputsReportTest()
        {
            // arrange
            StringWriter sw = new StringWriter();            
            string expected = File.ReadAllText("IntInvalidInputsReport.xml");
            // act
            comp.GetInvalidInputsReport(sw);
            string actual = sw.ToString();
            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem(@"TestFiles\IntInvalidInputsEmptyReport.xml")]
        [TestCategory("WithFiles")]
        [TestCategory("WithEmptyComp")]
        public void GetInvalidInputsEmptyReportTest()
        {
            // arrange
            StringWriter sw = new StringWriter();
            string expected = File.ReadAllText("IntInvalidInputsEmptyReport.xml");
            // act
            compEmpty.GetInvalidInputsReport(sw);
            string actual = sw.ToString();
            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem(@"TestFiles\IntInvalidInputsReport.xml")]
        [DeploymentItem(@"TestFiles\IntInvalidInputsEmptyReport.xml")]
        [DeploymentItem(@"TestFiles\input.txt")]
        [TestCategory("WithFiles")]
        public void TestFilesTest()
        {
            new List<string>
            {
                "IntInvalidInputsReport.xml",
                "IntInvalidInputsEmptyReport.xml",
                "input.txt"
            }.ForEach(f => Assert.IsTrue(File.Exists(f), f + " not found"));
        }
    }
}