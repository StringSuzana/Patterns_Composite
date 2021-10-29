using System;
using System.Collections.Generic;

namespace Composite
{

    public class Program
    {
        /**
         *  <<<    -5 * (9/6) / (7+(2-1.5))   >>>
         * 
         *                (division)
         *                /        \ 
         *          (division)    (addition)
         *           /      \          /   \
         *         -5    (division)   7  (substraction)
         *                 /     \          /      \
         *                9       6        2       -1.5
         *                
         * **/
        static void Main(string[] args)
        {

            Func<double, double, double> MULTIPLY = (x, y) => x * y;
            Func<double, double, double> ADD = (x, y) => x + y;
            Func<double, double, double> SUBSTRACT = (x, y) => x - y;
            Func<double, double, double> DIVISION = (x, y) => x / y;

            var zero = new NumericNode(0.0);
            var one_five = new NumericNode(1.5);
            var two = new NumericNode(2.0);
            var minus_five = new NumericNode(-5.0);
            var six = new NumericNode(6.0);
            var seven = new NumericNode(7.0);
            var nine = new NumericNode(9.0);

            ArithmeticNode minus_five_times_nine_div_six = new ArithmeticNode(MULTIPLY, minus_five, new ArithmeticNode(DIVISION, nine, six));
            ArithmeticNode seven_plus_two_minus_one_five = new ArithmeticNode(ADD, seven, new ArithmeticNode(SUBSTRACT, two, one_five));

            ArithmeticNode divide_twenty_by_three = new ArithmeticNode(DIVISION, minus_five_times_nine_div_six, seven_plus_two_minus_one_five);

            ArithmeticExpressionComposite tree = new ArithmeticExpressionComposite();
            tree.insert_whole_expression(divide_twenty_by_three);
            Console.WriteLine(tree.calculate());
        }
    }
    /**The operand => Component can be a number, or another arithmetic expression.**/
    public abstract class Component
    {

        public virtual Component LeftNode { get; set; }
        public virtual Component RightNode { get; set; }
        public virtual double value { get; set; }
        public virtual bool is_composite { get; set; }

        public virtual double calculate_arithmetic_operation(Component node)
        {
            if (is_composite)
                throw new NotImplementedException();
            else
                throw new NotSupportedException("calculate_arithmetic_operation function not supported for leaf nodes");
        }

    }
    public class ArithmeticNode : Component
    {
        public override double value { get; set; }

        Func<double, double, double> operation;
        public override bool is_composite { get; set; }

        public override Component LeftNode { get; set; }
        public override Component RightNode { get; set; }

        public ArithmeticNode(Func<double, double, double> a, Component left_component, Component right_component)
        {
            this.operation = a;
            this.is_composite = true;
            this.LeftNode = left_component;
            this.RightNode = right_component;
        }

        public override double calculate_arithmetic_operation(Component root)
        {
            if (LeftNode.is_composite == false && RightNode.is_composite == false)
            {

                this.value = operation(LeftNode.value, RightNode.value);
                this.is_composite = false;
            }
            while (root.is_composite)
            {
                find_node_with_both_numeric_values(root).calculate_arithmetic_operation(root);
            }
            return root.value;
        }
        public Component find_node_with_both_numeric_values(Component root)
        {
            if (root == null)
                return null;

            var traverse_queue = new Queue<Component>();/**Breadth first search with queue**/
            traverse_queue.Enqueue(root);
            Component current = null;

            while (traverse_queue.Count > 0)
            {
                current = traverse_queue.Dequeue();
                if (current.LeftNode.is_composite == false & current.RightNode.is_composite == false)
                    return current;
                if (current.LeftNode != null & current.LeftNode.is_composite)
                    traverse_queue.Enqueue(current.LeftNode);
                if (current.RightNode != null & current.RightNode.is_composite)
                    traverse_queue.Enqueue(current.RightNode);
            }
            return current;

        }
    }
    public class NumericNode : Component
    {

        public override double value { get; set; }
        public override bool is_composite { get; set; }

        public NumericNode(double val)
        {
            this.is_composite = false;
            this.value = val;
        }
    }
    public class ArithmeticExpressionComposite
    {
        public Component Root { get; set; }
        public double calculate()
        {
            return Root.calculate_arithmetic_operation(Root);
        }
        public void insert_whole_expression(Component root_expression)
        {
            if (Root == null)
            {
                Root = root_expression;
            }
            else
            {
                throw new NotSupportedException("Can not make a tree");
            }
        }

    }

}
/***
 * 
 *   public class Employee {
 *      private String name;
 *      private String dept;
 *      private int salary;
 *      private List<Employee> subordinates;
 *   
 *      // constructor
 *      public Employee(String name,String dept, int sal) {
 *         this.name = name;
 *         this.dept = dept;
 *         this.salary = sal;
 *         subordinates = new ArrayList<Employee>();
 *      }
 *   
 *      public void add(Employee e) {
 *         subordinates.add(e);
 *      }
 *   
 *      public void remove(Employee e) {
 *         subordinates.remove(e);
 *      }
 *   
 *      public List<Employee> getSubordinates(){
 *        return subordinates;
 *      }
 *   
 *      public String toString(){
 *         return ("Employee :[ Name : " + name + ", dept : " + dept + ", salary :" + salary+" ]");
 *      }   
 *   }
 *   
 *   
 *   public class CompositePatternDemo {
 *      public static void main(String[] args) {
 *  
 *     Employee CEO = new Employee("John","CEO", 30000);
 *
 *     Employee headSales = new Employee("Robert","Head Sales", 20000);
 *
 *     Employee headMarketing = new Employee("Michel","Head Marketing", 20000);
 *
 *     Employee clerk1 = new Employee("Laura","Marketing", 10000);
 *     Employee clerk2 = new Employee("Bob","Marketing", 10000);
 *
 *     Employee salesExecutive1 = new Employee("Richard","Sales", 10000);
 *     Employee salesExecutive2 = new Employee("Rob","Sales", 10000);
 *
 *     CEO.add(headSales);
 *     CEO.add(headMarketing);
 *
 *     headSales.add(salesExecutive1);
 *     headSales.add(salesExecutive2);
 *
 *     headMarketing.add(clerk1);
 *     headMarketing.add(clerk2);
 *
 *     //print all employees of the organization
 *     System.out.println(CEO); 
 *     
 *     for (Employee headEmployee : CEO.getSubordinates()) {
 *        System.out.println(headEmployee);
 *        
 *        for (Employee employee : headEmployee.getSubordinates()) {
 *           System.out.println(employee);
 *        }
 *     }		
 *  }
 *}
 *   
 ***/