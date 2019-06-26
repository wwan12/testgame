public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
public delegate void Callback<T, U, V,E>(T arg1, U arg2, V arg3,E arg4);


public delegate R CallbackReturn<T, R>(T arg1);
public delegate R CallbackReturn<T, U, V, E,R>(T arg1, U arg2, V arg3, E arg4);