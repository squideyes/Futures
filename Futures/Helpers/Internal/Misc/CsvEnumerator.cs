// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections;

namespace SquidEyes.Futures;

internal class CsvEnumerator : IEnumerable<string[]>, IDisposable
{
    private readonly StringReader reader;
    private readonly int expectedFields;

    private bool skipFirst;

    public CsvEnumerator(
        string csv, int expectedFields, bool skipFirst = false)
    {
        reader = new StringReader(csv);
        this.expectedFields = expectedFields;
        this.skipFirst = skipFirst;
    }

    public void Dispose()
    {
        reader?.Dispose();

        GC.SuppressFinalize(this);
    }

    public IEnumerator<string[]> GetEnumerator()
    {
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            var fields = line.Split(',');

            if (fields.Length != expectedFields)
            {
                throw new InvalidDataException(
                    $"{expectedFields} expected; {fields.Length} found");
            }

            if (skipFirst)
            {
                skipFirst = false;

                continue;
            }

            yield return fields;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}