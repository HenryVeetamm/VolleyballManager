import type { IAnnouncement } from "@/domain/IAnnouncement";
import { BaseService } from "./BaseService";

export class AnnouncementService extends BaseService<IAnnouncement>{
    
    constructor() {
        super("announcement");
        
    }
   
}