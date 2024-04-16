using System;


namespace GROUP1_MP2
{
    class Program
    {
        static void Main(string[] args){
            Dictionary<string,double> bisectIter = bisectIteration(0,1,6,"x^3-3x+1");
            Console.WriteLine(bisectIter["Root"]);
            Console.WriteLine(bisectIter["Function"]);
            Console.WriteLine(bisectIter["Epsilon"]);
            Dictionary<string,double> bisectError = bisectEpsilon(0,1,0.05,"x^3-3x+1");
            Console.WriteLine(bisectError["Root"]);
            Console.WriteLine(bisectError["Function"]);
            Console.WriteLine(bisectError["Iteration"]);
        }

        static double evaluation(string polynomial, double x){
            
            //fix polynomial string to be split using "+"
            string formattedPolynomial = polynomial.Replace(" ", "").Replace("-","+-");
            string[] termCollection = formattedPolynomial.Split("+");
            
            //initialize result to 0  for summation
            double result = 0;

            //iterate the terms in the termCollection
            foreach(string term in termCollection){
                int xIndex = term.IndexOf("x");

                //if term is empty then ignore it
                if(term == ""){
                    continue;
                }

                //check if term is constant
                if(!term.Contains("x")){
                    result += double.Parse(term);
                    continue;
                }

                //check if it is not transcendental
                if(!(term.Contains("sin")|term.Contains("cos")|term.Contains("tan")|term.Contains("csc")|term.Contains("sec")|term.Contains("cot"))){
                   //evaluate term
                    double termEval = termEvaluate(term, x);
                    result+=termEval;
                }

                //solve if term is transcendental
                else{
                    if(term.Contains("sin")){
                        string transformedTerm = term.Replace("sin", "").Replace(")","").Replace("(","");
                        result+=Math.Sin(termEvaluate(transformedTerm,x));
                    }
                    else if(term.Contains("cos")){
                        string transformedTerm = term.Replace("cos", "").Replace(")","").Replace("(","");
                        result+=Math.Cos(termEvaluate(transformedTerm,x));
                    }
                    else if(term.Contains("tan")){
                        string transformedTerm = term.Replace("tan", "").Replace(")","").Replace("(","");
                        result+=Math.Tan(termEvaluate(transformedTerm,x));
                    }
                    else if(term.Contains("csc")){
                        string transformedTerm = term.Replace("csc", "").Replace(")","").Replace("(","");
                        result+=1/Math.Sin(termEvaluate(transformedTerm,x));
                    }
                    else if(term.Contains("sec")){
                        string transformedTerm = term.Replace("sec", "").Replace(")","").Replace("(","");
                        result+=1/Math.Cos(termEvaluate(transformedTerm,x));
                    }
                    else if(term.Contains("cot")){
                        string transformedTerm = term.Replace("cot", "").Replace(")","").Replace("(","");
                        result+=1/Math.Tan(termEvaluate(transformedTerm,x));
                    }
                }

            }


            
            return result;
        }

        static double termEvaluate(string term, double x){
            //initialize coefficient and exponent string to "" to add each char
                    string coefficientString = "";
                    string exponentString = "";
                    double result = 0;
                    int xIndex = term.IndexOf("x");

                    //iterate through the term string
                    for(int i = 0; i < term.Length; i++){

                        //add left side of the xIndex to the coefficient char collection
                        if(i<xIndex){
                            coefficientString+=term[i];

                        //add right side of the xIndex to the exponent char collection 
                        }else if(i>xIndex){
                            exponentString+=term[i];
                        }

                        //parse string collections to double
                        exponentString = exponentString.Replace("^", "");
                        double coefficient;
                        double exponent;
                        
                        try{
                            coefficient = double.Parse(coefficientString);
                        }catch{
                            coefficient = 1;
                        }
                        
                        try{
                            exponent = double.Parse(exponentString);
                        }
                        catch{
                            exponent = 1;
                        }

                        result += coefficient * Math.Pow(x, exponent); 
                    }
            return result;
        }

        static Dictionary<string, double> bisectIteration(double a, double b, int maxIter, String stringFunc)
        {
            double c = 0;
            double epsilon = 0;

            for (int i = 0; i < maxIter; i++)
            {
                c = (a + b) / 2;



                double cFunc = evaluation(stringFunc, c);
                double aFunc = evaluation(stringFunc, a);
                double bFunc = evaluation(stringFunc, b);

                epsilon = Math.Abs(a - b);

                if (aFunc * bFunc > 0 || a == b)
                {
                    throw new ArgumentException("f(a) and f(b) must have opposite sign");
                }

                else
                {
                    if (cFunc < 0 && aFunc < 0 || cFunc > 0 && aFunc > 0)
                    {
                        a = c;
                        epsilon = Math.Abs(a - b);
                        cFunc = evaluation(stringFunc, c);
                        aFunc = evaluation(stringFunc, a);
                        bFunc = evaluation(stringFunc, b);


                    }

                    else if (cFunc < 0 && bFunc < 0 || cFunc > 0 && bFunc > 0)
                    {
                        b = c;
                        epsilon = Math.Abs(a - b);
                        cFunc = evaluation(stringFunc, c);
                        aFunc = evaluation(stringFunc, a);
                        bFunc = evaluation(stringFunc, b);

                    }
                }
            }
            Dictionary<string,double> result = new Dictionary<string, double>{
                {"Root", c},
                {"Function", evaluation(stringFunc,c)},
                {"Epsilon", epsilon}
            };
            return result;
        }

        static Dictionary<string,double> bisectEpsilon(double a, double b, double error, String stringFunc, int maxIter = 1000 )
        {
            double c = 0;
            int counter = 1;

            for (int i = 0; i < maxIter; i++)
            {
                double aFunc = evaluation(stringFunc, a);
                double bFunc = evaluation(stringFunc, b);

                double checker = Math.Abs(a - b);
                
                if (aFunc * bFunc > 0 || a == b)
                {
                    throw new ArgumentException("f(a) and f(b) must have opposite sign or a and b must not bee equals must not be greater than b");
                }

                if (checker < error)
                {
                    break;
                }

                

                c = (a + b) / 2;

                double cFunc = evaluation(stringFunc, c);
                if (cFunc < 0 && aFunc < 0 || cFunc > 0 && aFunc > 0)
                {
                    a = c;
                    checker = Math.Abs(a - b);
                    cFunc = evaluation(stringFunc, c);
                    aFunc = evaluation(stringFunc, a);
                    bFunc = evaluation(stringFunc, b);


                }

                else if (cFunc < 0 && bFunc < 0 || cFunc > 0 && bFunc > 0)
                {
                    b = c;
                    checker = Math.Abs(a - b);
                    cFunc = evaluation(stringFunc, c);
                    aFunc = evaluation(stringFunc, a);
                    bFunc = evaluation(stringFunc, b);




                }
                counter++;
            }
            Dictionary<string,double> result = new Dictionary<string, double>{
                {"Root", c},
                {"Function", evaluation(stringFunc,c)},
                {"Iteration", counter}
            };
            return result;
        }
    }
}
