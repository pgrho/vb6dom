namespace Shipwreck.VB6Models.Parsing
{
    internal interface ITokenMatcherItemGroup<TGroup>
    {
        TGroup AddItem(TokenMatcherItemBase item);
    }
}