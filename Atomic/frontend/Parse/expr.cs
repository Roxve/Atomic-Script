using System;
using System.Linq;
using System.Collections.Generic;
using Atomic_AST;
using System.Diagnostics.CodeAnalysis;
namespace Atomic_lang;

public partial class Parser
{         
	private Expression parse_assigment_expr()
	{
		var left = this.parse_obj_expr();

		if (this.at().type == TokenType.setter)
		{
			this.take();
			var value = this.parse_assigment_expr();

			AssignmentExpr expr = Create<AssignmentExpr>();
			expr.value = value;
			expr.assigne = left;
			return expr;
		}
		return left;
	}



	private Expression parse_obj_expr()
	{
		if (this.at().type != TokenType.OpenBrace)
		{
			return this.parse_compare_type1_expr();
		}

		this.take();
		var properties = new List<Property>();
		while (this.NotEOF() && this.at().type != TokenType.CloseBrace)
		{
			var key = this.except(TokenType.id).value;


			Property property =  Create<Property>();
			if (this.at().type == TokenType.Comma)
			{
				this.take();
				property.key = key;
				properties.Add(property);
				continue;
			}

			else if (this.at().type == TokenType.CloseBrace)
			{
				property.key = key;
				properties.Add(property);
				continue;
			}

			this.except(TokenType.Colon);

			var value = this.parse_expr();

			property.key = key; property.value = value;

			properties.Add(property);

			if (this.at().type != TokenType.CloseBrace)
			{
				this.except(TokenType.Comma);
			}
		}
		this.except(TokenType.CloseBrace);

		ObjectLiteral Obj = Create<ObjectLiteral>();

		Obj.properties = properties;
		return Obj;
	}
    //handles &/|/&&/||
	private Expression parse_compare_type1_expr()
	{
		var left = this.parse_compare_type2_expr();
		while (this.at().value == "&" || this.at().value == "&&" || this.at().value == "|" || this.at().value == "||")
		{
			var Opeartor = Create<BinaryOperator>();
			Opeartor.value = this.take().value;
			
			var right = this.parse_compare_type2_expr();
			var BE = Create<BinaryExpression>();
			
			BE.left = left;
			BE.right = right;
			BE.Operator = Opeartor;
			left = BE;
		}
		return left;
	}

	//handles =/</>
	private Expression parse_compare_type2_expr()
	{
		var left = this.parse_additive_expr();
		while (this.at().value == "=" || this.at().value == "<" || this.at().value == ">")
		{
			var Opeartor = Create<BinaryOperator>();
			Opeartor.value = this.take().value;
			
			var right = this.parse_additive_expr();
			var BE = Create<BinaryExpression>();
			
			BE.left = left;
			BE.right = right;
			BE.Operator = Opeartor;
			left = BE;
		}
		return left;
	}
	// handels additive ane subtraction operations
	private Expression parse_additive_expr()
	{
		var left = this.parse_multiplicitave_expr();
		while (this.at().value == "+" || this.at().value == "-")
		{
			
			var Opeartor = Create<BinaryOperator>();
			Opeartor.value = this.take().value;
			
			var right = this.parse_multiplicitave_expr();
			var BE = Create<BinaryExpression>();
			
			BE.left = left;
			BE.right = right;
			BE.Operator = Opeartor;
			left = BE;
		}
		return left;
	}



	// handels multiplicitave and divison operations
	private Expression parse_multiplicitave_expr()
	{
		var left = this.parse_primary_expr();
		while (this.at().value == "/" || this.at().value == "*" || this.at().value == "%")
		{
			var Opeartor = Create<BinaryOperator>();
			Opeartor.value = this.take().value;
			
			var right = this.parse_primary_expr();
			var BE = Create<BinaryExpression>();
			
			BE.left = left;
			BE.right = right;
			BE.Operator = Opeartor;
			left = BE;
		}
		return left;
	}
	
    private Expression parse_primary_expr()
	{
		IonType token = this.at().type;
		switch (token)
		{
			case IonType.id:
				Identifier id = Create<Identifier>();
				id.symbol = this.take().value;
				return id;
			case IonType.num_type:
				NumericLiteral num = Create<NumericLiteral>();
				num.value = Convert.ToInt32(take().value);
				return num;
			case IonType.null_type:
				NullLiteral Null = Create<NullLiteral>();
				this.take();
				return Null;
			case IonType.str_type:
				StringLiteral str = Create<StringLiteral>();
				str.value = take().value;
				return str;
			case IonType.bool_type:
				Bool Bool = Create<Bool>();
				Bool.value = Convert.ToBoolean(this.take().value);
				return Bool;
			case IonType.OpenParen:
				this.take();
				var value = parse_expr();
				except(IonType.CloseParen);
				return value;
			default:
				error("Unexpected ion found during parsing! ",this.at());
				this.take();
				return Create<NullLiteral>();
		}
	}
}
