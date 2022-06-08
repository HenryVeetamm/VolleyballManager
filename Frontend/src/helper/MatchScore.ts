export default function checkMatchScore(score : string) : boolean {
    const pattern = /^[0-9]{2}\:[0-9]{2}/
    var splitted = score.split(",")
    for (const set of splitted) {
     
        if (!pattern.test(set)){
            
            return false;
        }
    }
    return true
}
