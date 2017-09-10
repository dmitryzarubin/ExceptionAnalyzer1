using System;
using System.Text;
using System.IO;
using Advitex.ExceptionAnalizer;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Advitex.TST
{
    public class MyEntity
    {
        /// <summary>
        /// ...
        /// </summary>
        /// <exception cref= "ArgumentException" > Thrown when value of name parameter is null </exception>
        /// <exception cref= "ArgumentException" > Thrown when value of surname parameter is null </exception>
        public MyEntity(int id, string name, string surname, string midlname)
        {
            CheckNotNull(name, "PersonName");
            CheckNotNull(surname, "PersonSurname");

            try
            {
                throw new InvalidOperationException("Test1");
            }
            catch (InvalidOperationException ex)
            {
                CheckNotNull(name, "PersonName");

                throw ex;
            }
        }


        /// <summary>
        /// ...
        /// </summary>
        [ExceptionContract(
            ExceptionType = typeof(ArgumentException),
            Description = "Thrown when value of {name-of:val} parameter is null")]
        public void CheckNotNull(string val, string paramName)
        {
            if (val == null)
                throw new ArgumentException(string.Format("Value of {0} parameter is null", paramName));
        }

    }

    public interface IMy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException">Throw when ...</exception>
        void DoSomething();
    }



    public class Class1 : IDisposable
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <exception cref="ArgumentException">A is zero</exception>
        /// <exception cref="InvalidOperationException">11</exception>
        /// <exception cref="InvalidOperationException">22</exception>
        /// <exception cref="NotImplementedException">33</exception>
        /// <exception cref="InvalidOperationExceptionERR">Throw when ...</exception>
        public Class1(int a)
        {
            

            if (a == 0)
                throw new ArgumentException("A is zero");

            if (a == 0)
            throw new InvalidOperationException("11");

            if (a == 0)
                throw new InvalidOperationException("22");

            A = a;
        }

        ~Class1()
        {
            Method1();
            throw new Exception();
        }


        /// <summary>
        /// ...
        /// </summary>
        /// <exception cref="Exception">Throw when 1 ...</exception>
        /// <exception cref="Exception">Throw when 2 ...</exception>
        /// <exception cref="ArgumentException">Throw when ...</exception>
        public void Dispose()
        {
            Method1();
            throw new Exception();
        }


        public void Method()
        {
            
            try
            {
                var s = string.Format("{0}", 1);
            }
            finally 
            {
                
                throw new Exception();
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <exception cref="Exception">Throw when 1 ...</exception>
        /// <exception cref="Exception">Throw when 2 ...</exception>
        /// <exception cref="ArgumentException">Throw when ...</exception>
        public void Method1()
        {
            throw new Exception();
        }

        public int A { get; private set; }
    }

    public class MyException : Exception
    {
        public MyException()
        {
            var a = new C();
            a.Main();
        }
    }

    class C
    {


        ///<summary>
        /// MySummary 11
        /// MySummary 22
        ///</summary>
        public void Main()
        {
            var aa = new Class1(1);

            var ex = new ArgumentException("Not assigned 3");
            var sb = new StringBuilder();
            sb.AppendLine("");
            GetEx2();


            var s = string.Format("{0}", 1);
            Console.WriteLine(s);


            using (var x = new MemoryStream())
            {

            }


            //throw new ArgumentException1("Unresolved exception");
            //throw new MyException("Unresolved exception");

            try
            {
                throw new ArgumentException("Not assigned 1");
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                throw new System.ArgumentException("Not assigned 2");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                throw new System.ArgumentNullException("Not assigned 3");
            }
            catch (ArgumentException)
            {
                GetEx2();
                throw;
            }

            try
            {
                throw new System.ArgumentNullException("Not assigned 4");
            }
            catch (Exception)
            {
            }

            try
            {
                throw new System.ArgumentNullException("Not assigned 5");
            }
            catch
            {
            }


            try
            {
                try
                {
                    throw new System.ArgumentNullException("Not assigned 5");
                }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException("Not assigned 5");
                }
            }
            catch (ArgumentException)
            {
            }

            throw ex;
            throw GetEx();
        }


        /// <summary>
        /// ...
        /// </summary>
        /// <exception cref="ArgumentException">Throw when ...</exception>
        public void GetEx2()
        {
            if (1 > 0)
                throw new ArgumentException();
        }

        public Exception GetEx()
        {
            return new ArgumentException("Not assigned 4");
        }

    }
}