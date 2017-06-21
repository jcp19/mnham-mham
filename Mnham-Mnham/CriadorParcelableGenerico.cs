using System;
using Android.OS;

namespace Mnham_Mnham
{
    public sealed class CriadorParcelableGenerico<T> : Java.Lang.Object, IParcelableCreator
        where T : Java.Lang.Object, new()
    {
        private readonly Func<Parcel, T> funCreate;

        public CriadorParcelableGenerico(Func<Parcel, T> createFromParcelFunc)
        {
            funCreate = createFromParcelFunc;
        }

        #region IParcelableCreator Implementation

        public Java.Lang.Object CreateFromParcel(Parcel source)
        {
            return funCreate(source);
        }

        public Java.Lang.Object[] NewArray(int size)
        {
            return new T[size];
        }

        #endregion
    }
}