using System.Collections.Generic;

public class IngredientList : List<Ingredient>
{
    public void Push(Ingredient ing)
    {
        this.Add(ing);
    }

    public Ingredient Peek()
    {
        return this[Count - 1];
    }

    public Ingredient Pop()
    {
        var pop = this[Count - 1];
        Remove(pop);
        return pop;
    }
}