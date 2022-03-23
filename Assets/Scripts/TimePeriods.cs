public class TimePeriods {

    public string getTimePeriod(float blockCount){
        
        if(blockCount > 0f && blockCount < 300f)
            return "Volcanic Terrain";
        
        else if(blockCount > 300f)
            return "Grass Terrain";
        
        return "";

    }
}