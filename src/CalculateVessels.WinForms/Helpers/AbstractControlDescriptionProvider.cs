using System.ComponentModel;

namespace CalculateVessels.Helpers;

public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
{
    public AbstractControlDescriptionProvider()
        : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
    {
    }

    public override Type GetReflectionType(Type objectType, object? instance)
    {
        if (objectType == typeof(TAbstract))
            return typeof(TBase);

        return base.GetReflectionType(objectType, instance);
    }

    public override object? CreateInstance(IServiceProvider? provider, Type objectType, Type[]? argTypes, object?[]? args)
    {
        if (objectType == typeof(TAbstract))
            objectType = typeof(TBase);

        return base.CreateInstance(provider, objectType, argTypes, args);
    }
}

public class AbstractGenericFormDescriptionProvider : TypeDescriptionProvider
//where T : class, IInputData

{
    public AbstractGenericFormDescriptionProvider()
        : base()//TypeDescriptor.GetProvider(typeof(BaseCalculateForm<T>)))
    {
    }

    public override Type GetReflectionType(Type objectType, object? instance)
    {
        return typeof(Form);
    }

    public override object? CreateInstance(IServiceProvider? provider, Type objectType, Type[]? argTypes, object?[]? args)
    {
        objectType = typeof(Form);

        return base.CreateInstance(provider, objectType, argTypes, args);
    }
}