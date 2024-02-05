namespace Euler_Logic.Helpers;

public static class Mod {
    public static int NegativeMod(int num, int div) {
        return (num % div + div) % div;
    }
}
