#include "TestCpp.h"

using namespace System;
using namespace System::Threading;

TestCpp::TestCpp()
{
	return;
}

TestCpp::~TestCpp()
{
}

void TestCpp::T1()
{
	while (true)
	{
		System::Console::WriteLine((gcnew Random())->Next(15));
	}
}

void TestCpp::T1Async()
{
	Thread^ t = gcnew Thread(gcnew ThreadStart(this, &TestCpp:: T1));
	t->Start();
}

void TestCpp::T0()
{
	T1Async();
}
