public class TimePeriods {

    public string getTimePeriod(float blockCount){
        
        if(blockCount > 0f && blockCount < 100f)
            return "Volcanic Terrain";
        
        if(blockCount >= 100f && blockCount <= 500f)
            return "Grass Terrain";
        
        return "";

    }
}