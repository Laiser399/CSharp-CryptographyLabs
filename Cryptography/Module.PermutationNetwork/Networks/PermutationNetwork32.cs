namespace Module.PermutationNetwork.Networks
{
    class Swapper32 : PermutationNetwork32
    {
        internal Swapper32(byte[] permutation, int phase)
        {
            Masks.Add((permutation[0] == 1) ? 1u << phase : 0);
        }
    }

    public class PermutationNetwork32
    {
        // The list of masks for the final permutation
        public List<uint> Masks { get; set; }

        protected PermutationNetwork32()
        {
            Masks = new List<uint>();
        }

        public PermutationNetwork32(byte[] permutation) : this()
        {
            if (!CheckPermutation(permutation))
            {
                throw new ArgumentException("Invalid permutation to PermutationNetwork");
            }

            Initialize(permutation, 0);
        }

        private static bool CheckPermutation(byte[] permutation)
        {
            if (permutation.Length != 32)
            {
                return false;
            }

            int check = 0;
            for (var i = 0; i < 32; i++)
            {
                check |= 1 << i;
            }

            return check == -1;
        }

        private PermutationNetwork32(byte[] permutation, int phaseParm) : this()
        {
            Initialize(permutation, phaseParm);
        }

        private void Initialize(byte[] permutation, int phase)
        {
            PermutationNetwork32 permutationNetworkEven;
            PermutationNetwork32 permutationNetworkOdd;
            var size = permutation.Length;
            var delta = 32 / size;

            // Get the wiring needed for our two inner networks
            var permutationsInner =
                GetInnerPermutations(permutation, delta, phase, out var inputMask, out var outputMask);

            // Recursively create those two inner networks
            if (size == 4)
            {
                // The recursion ends here by creating two swappers rather than normal permutation networks
                permutationNetworkEven = new Swapper32(permutationsInner[0], phase);
                permutationNetworkOdd = new Swapper32(permutationsInner[1], phase + delta);
            }
            else
            {
                // For larger sizes, the inner networks are standard permutation networks
                permutationNetworkEven = new PermutationNetwork32(permutationsInner[0], phase);
                permutationNetworkOdd = new PermutationNetwork32(permutationsInner[1], phase + delta);
            }

            // Extract the masks from our inner permutation networks into our list of masks
            ExtractMasks(permutationNetworkEven, permutationNetworkOdd, inputMask, outputMask);
        }

        private static byte[][] GetInnerPermutations(
            byte[] permutationOuter,
            int delta,
            int phase,
            out uint inputMask,
            out uint outputMask)
        {
            inputMask = 0;
            outputMask = 0;

            var permutationsInner = new byte[2][];
            var size = permutationOuter.Length;
            permutationsInner[0] = new byte[size / 2];
            permutationsInner[1] = new byte[size / 2];

            var reversePermutation = GetReversePermutation(permutationOuter);

            // We map out cycles until we've covered all the inputs and their
            // connections to the outputs.  mappedToDest keeps track of which
            // inputs have been mapped and cMapped keeps track of the count of
            // mapped inputs.  When we end a cycle, if cMapped == size then
            // we're done.  If it's not, then we find an unmapped input and
            // map out the cycle starting from that input.
            var mappedToDest = new bool[size];
            var cMapped = 0;

            // While there are cycles to map...
            while (cMapped < size)
            {
                var fInput = true;
                byte network = 0;
                byte startPin;

                // Find the next unmapped input
                for (startPin = 0; startPin < size; startPin++)
                {
                    if (!mappedToDest[startPin])
                    {
                        break;
                    }
                }

                // Keep track of our starting pin
                var cycleStartPin = startPin;

                // Adjacent pairs of pins are connected to a switcher which
                // may or may not swap them around.  Get the index for the
                // switcher we're attached to.
                var switcher = (byte)(startPin / 2);

                // This is the pin we need to be mapped to
                var endPin = permutationOuter[startPin];

                // Trace the cycle starting at startPin
                while (true)
                {
                    // We alternate from input to output of the current permutation network
                    fInput = !fInput;

                    // Where we came from
                    var switcherPrev = switcher;
                    switcher = (byte)(endPin / 2);

                    // If we're connected to the wrong network currently...
                    if ((endPin & 1) != network)
                    {
                        // If we're on the input side...);
                        if (fInput)
                        {
                            // Swap the two input connections around
                            inputMask = AddSwap(inputMask, switcher, delta, phase);
                        }
                        else
                        {
                            // Swap the two output connections around
                            outputMask = AddSwap(outputMask, switcher, delta, phase);
                        }
                    }

                    // Mark this input bit as mapped
                    mappedToDest[fInput ? endPin : startPin] = true;

                    // mark the connections that need to be made in the inner network
                    permutationsInner[network][fInput ? switcher : switcherPrev] = fInput ? switcherPrev : switcher;

                    // We've mapped one more pin
                    cMapped++;

                    // Get the other pin on this switcher
                    startPin = (byte)(endPin ^ 1);

                    // If we're back to startPin on the input side, then we've completed the cycle
                    if (startPin == cycleStartPin && fInput)
                    {
                        break;
                    }

                    // The other pin on our switcher has to go to the other network
                    network = (byte)(1 - network);

                    // The pin we map to on the other side of the network
                    endPin = fInput ? permutationOuter[startPin] : reversePermutation[startPin];
                }
            }

            return permutationsInner;
        }

        private static byte[] GetReversePermutation(byte[] permutation)
        {
            var size = permutation.Length;
            var reversePermutation = new byte[size];
            for (byte i = 0; i < size; i++)
            {
                reversePermutation[permutation[i]] = i;
            }

            return reversePermutation;
        }

        private static uint AddSwap(uint maskCur, int switcher, int delta, int phase)
        {
            return maskCur | (1u << (2 * switcher * delta + phase));
        }

        private void ExtractMasks(PermutationNetwork32 even, PermutationNetwork32 odd, uint inputMask, uint outputMask)
        {
            var masksEven = even.Masks;
            var masksOdd = odd.Masks;
            Masks.Add(inputMask);
            for (var i = 0; i < masksEven.Count; i++)
            {
                Masks.Add(masksEven[i] | masksOdd[i]);
            }

            Masks.Add(outputMask);
        }
    }
}