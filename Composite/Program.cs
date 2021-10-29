using System;
using System.Collections.Generic;

namespace Composite
{

    public class Program
    {


        static void Main(string[] args)
        {
            Func<double, double, double> MULTIPLY = (x, y) => x * y;
            Func<double, double, double> ADD = (x, y) => x + y;
            Func<double, double, double> SUBSTRACT = (x, y) => x - y;
            Func<double, double, double> DIVISION = (x, y) => x / y;

            var zero = new NumericNode(0.0);
            var one = new NumericNode(1.0);
            var two = new NumericNode(2.0);
            var three = new NumericNode(3.0);
            var five = new NumericNode(5.0);

            ArithmeticNode add_one_and_two = new ArithmeticNode(ADD, one, two);
            ArithmeticNode add_one_and_three = new ArithmeticNode(ADD, one, add_one_and_two);
            ArithmeticNode multiply_four_and_five = new ArithmeticNode(MULTIPLY, add_one_and_three, five);


            ArithmeticNode divide_twenty_by_zero = new ArithmeticNode(DIVISION, multiply_four_and_five, zero);
            ArithmeticNode divide_twenty_by_three = new ArithmeticNode(DIVISION, multiply_four_and_five, three);

            ArithmeticExpressionComposite tree = new ArithmeticExpressionComposite();
            tree.insert_whole_expression(divide_twenty_by_three);
            Console.WriteLine(tree.calculate());
        }
    }
    //The operand => Component can be a number, or another arithmetic expression.
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
